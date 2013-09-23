/*
 * 2D Platform Game Creator
 * Copyright (C) Milan Rybář. All rights reserved.
 * 
 * -----------------------------------------------------------------------------
 * 
 * Based on http://create.msdn.com/en-US/education/catalog/sample/winforms_series_2
 */
//-----------------------------------------------------------------------------
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PlatformGameCreator.Editor.Xna
{
    /// <summary>
    /// Container class implements the IServiceProvider interface. This is used
    /// to pass shared services between different components, for instance the
    /// ContentManager uses it to locate the IGraphicsDeviceService implementation.
    /// </summary>
    public class ServiceContainer : IServiceProvider
    {
        private Dictionary<Type, object> services = new Dictionary<Type, object>();



        /// <summary>
        /// Adds a new service to the collection.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <param name="service">The service to add.</param>
        public void AddService<T>(T service)
        {
            services.Add(typeof(T), service);
        }


        /// <summary>
        /// Looks up the specified service.
        /// </summary>
        /// <param name="serviceType">Type of the service to look up.</param>
        public object GetService(Type serviceType)
        {
            object service;

            services.TryGetValue(serviceType, out service);

            return service;
        }
    }
}
