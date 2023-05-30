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

namespace BobSuperStores.Modules;

using Autofac;
using BobSuperStores.Data;
using BobSuperStores.Data.Csv;

using JetBrains.Annotations;

using Serilog;

/// <summary>
/// The main module to register the logger and the options.
/// </summary>
public class MainModule : Module
{
    #region Fields

    [NotNull]
    private readonly ILogger _logger;

    [NotNull]
    private readonly IValidatableCommandLineOptions _commandLineOptions;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainModule"/> class.
    /// </summary>
    /// <param name="logger">The main logger to register.</param>
    /// <param name="commandLineOptions">The command line options to register.</param>
    public MainModule([NotNull] ILogger logger, [NotNull] IValidatableCommandLineOptions commandLineOptions)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._commandLineOptions = commandLineOptions ?? throw new ArgumentNullException(nameof(commandLineOptions));
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterInstance(this._logger).As<ILogger>().ExternallyOwned().SingleInstance();
        builder.RegisterInstance(this._commandLineOptions).AsSelf();
    }

    #endregion
}