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

namespace BobSuperStores.Data.Modules;

using Autofac;

using BobSuperStores.Data.Builder;
using BobSuperStores.Data.Calculations;
using BobSuperStores.Data.Json;
using BobSuperStores.Data.Model;
using BobSuperStores.Data.Transformation;

using JetBrains.Annotations;

using Serilog;


/// <summary>
/// Registers the dependencies, when the data is loaded from JSON files.
/// </summary>
internal class FromJsonModule : Module
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FromJsonModule"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public FromJsonModule([CanBeNull] ILogger logger = null)
    {
        this.Logger = logger;
    }

    #endregion

    #region Properties

    [CanBeNull]
    private ILogger Logger { get; }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        this.Logger?.Debug("Register {0} as {1}", nameof(DefaultDataContextBuilder), nameof(IDataContextBuilder));
        builder.RegisterType<DefaultDataContextBuilder>().As<IDataContextBuilder>();
        this.Logger?.Debug("Register {0} as {1}", nameof(JsonDataSource<SuperStore>), nameof(IDataSource<SuperStore>));
        this.Logger?.Debug("Register {0} as {1}", nameof(JsonDataSource<Warehouse>), nameof(IDataSource<Warehouse>));
        builder.RegisterGeneric(typeof(JsonDataSource<>)).As(typeof(IDataSource<>));
        this.Logger?.Debug("Register {0} as {1}", nameof(AllDistancesDataSource), nameof(IDataSource<Distance>));
        builder.RegisterType<AllDistancesDataSource>().As<IDataSource<Distance>>();
        this.Logger?.Debug("Register {0} as {1}", nameof(DefaultDataTransformation), nameof(IDataTransformation));
        builder.RegisterType<DefaultDataTransformation>().As<IDataTransformation>();
        this.Logger?.Debug("Register {0} as {1}", nameof(ConvertWarehouseOpeningCostsTransformationStep), nameof(IDataTransformationStep));
        builder.RegisterType<ConvertWarehouseOpeningCostsTransformationStep>().As<IDataTransformationStep>();
        this.Logger?.Debug("Register {0} as {1}", nameof(HaversineFormulaDistanceCalculator), nameof(IDistanceCalculator));
        builder.RegisterType<HaversineFormulaDistanceCalculator>().As<IDistanceCalculator>().SingleInstance();
    }

    #endregion

}

