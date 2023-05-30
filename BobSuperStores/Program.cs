#region Copyright (c) OPTANO GmbH

// ////////////////////////////////////////////////////////////////////////////////
// 
//        OPTANO GmbH Source Code
//        Copyright (c) 2010-2023 OPTANO GmbH
//        ALL RIGHTS RESERVED.
// 
//    The entire contents of this file is protected by German and
//    International Copyright Laws. Unauthorized reproduction,
//    reverse-engineering, and distribution of all or any portion of
//    the code contained in this file is strictly prohibited and may
//    result in severe civil and criminal penalties and will be
//    prosecuted to the maximum extent possible under the law.
// 
//    RESTRICTIONS
// 
//    THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES
//    ARE CONFIDENTIAL AND PROPRIETARY TRADE SECRETS OF
//    OPTANO GMBH.
// 
//    THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED
//    FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE
//    COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE
//    AVAILABLE TO OTHER INDIVIDUALS WITHOUT WRITTEN CONSENT
//    AND PERMISSION FROM OPTANO GMBH.
// 
// ////////////////////////////////////////////////////////////////////////////////

#endregion

using Autofac;

using BobSuperStores.Data;
using BobSuperStores.Data.Csv;
using BobSuperStores.Data.Json;
using BobSuperStores.Data.Optimization.OptanoModeling;
using BobSuperStores.Modules;

using CommandLine;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;

using Serilog;

const int ApplicationSuccess = 0;
const int CommandLineOptionsNotValid = 1;
const int ComputationFailed = 2;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = new ContainerBuilder();
return Parser.Default.ParseArguments<JsonCommandLineOptions>(args)
    .MapResult(
        csvOptions => ValidateCommandLineOptions(csvOptions) ? Run(builder, new MainModule(logger, csvOptions)) : CommandLineOptionsNotValid,
        _ => CommandLineOptionsNotValid);


bool ValidateCommandLineOptions(IValidatableCommandLineOptions commandLineOptions)
{
    logger.Information("Starting...");
    logger.Information("Passed Arguments: {0}", string.Join(" ", args));

    try
    {
        logger.Debug("Validating passed arguments...");
        commandLineOptions.Validate();
    }
    catch (ArgumentValidationException e)
    {
        logger.Error(e.Message);
        return false;
    }

    return true;
}

int Run([NotNull] ContainerBuilder builder, [NotNull] MainModule module)
{
    // register all modules
    builder.RegisterModule(module);
    builder.RegisterAssemblyModules(typeof(IProblemSolver).Assembly);

    // solve the facility location problem
    using var scope = builder.Build().BeginLifetimeScope();
    var solver = scope.Resolve<IProblemSolver>();
    var success = solver.HandleProblem();

    return success ? ApplicationSuccess : ComputationFailed;
}