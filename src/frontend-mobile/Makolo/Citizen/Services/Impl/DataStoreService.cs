//using Data.Impl;
using Microsoft.Extensions.Logging;
using Citizen.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Common;
using System.Threading.Tasks;
using Citizen.Models.Personal;
using Citizen.Operations;

namespace Citizen.Services.Impl
{
    public class DataStoreService: IDataStoreService
    {
        private double _initialized = 0;
        private Object _sync = new object();
        private ILoggingService logger;
        public const string LoggingPrefix = "Mobile.Services.Impl.DataStoreService::";

        private IEnumerable<Profile> _profile;
        private IEnumerable<SynchedPhysicalContacts> _synchedPhysicalContacts;
       // private IEnumerable<Contact> _contacts;
        private Dictionary<string, List<Object>> _tempCache;
        public DataStoreService() //ILoggerFactory loggerFactory,
        {
            _tempCache = new Dictionary<string, List<object>>();
            var logger = Xamarin.Forms.DependencyService.Get<ILoggingService>();
            this.logger = logger;
            this.logger.Debug($"${LoggingPrefix}ctr() - Start");
            var settingsService = Xamarin.Forms.DependencyService.Get<ISettingsService>();
            var loggerFactory = Xamarin.Forms.DependencyService.Get<ILoggerFactory>();
            var settings = settingsService.GetSettings();
            var storagePath = Framework.Files.FileSystemManager.ConstructFullPath("str", "db");
            var defaultConnectionString = Framework.Files.FileSystemManager.Combine(storagePath, settings.DefaultConnectionString);
            var salestConnectionString = Framework.Files.FileSystemManager.Combine(storagePath, settings.SalesConnectionString);


            Framework.Files.FileSystemManager.EnsureDirectoryCreated(storagePath);
            Framework.Files.FileSystemManager.EnsureFileCreated(defaultConnectionString);
            Framework.Files.FileSystemManager.EnsureFileCreated(salestConnectionString);

            OperationService = null; // new OperationsService(defaultConnectionString, salestConnectionString, loggerFactory);
            this.logger.Debug($"${LoggingPrefix}ctr() - End");
        }

        private void ProcessInitialization()
        {
            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Start - _initialized: ${_initialized}");
            if (this._initialized == 1)
                return;

            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Load storage .....");

            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading categories");
            /*var catResults = OperationService.UpdateCategories(null);
            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading categories results successfull? ${catResults.IsSuccess}");
            _categories = catResults.Results;
            this.logger.Debug(_categories);

            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading products");
            var productResults = OperationService.UpdateProducts(null);
            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading products results successfull? ${productResults.IsSuccess}");
            _products = productResults.Results;
            this.logger.Debug(_products); */

            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - process storage content");


            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Done initializing dataStore");
            Interlocked.Exchange(ref _initialized, 1);
            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - End");
        }
/*
        public IEnumerable<Contact> LoadContacts()
        {
            if (_contacts != null)
            {
                return _contacts;
            }

            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading contacts");
            var contactsResults = OperationService.UpdateContacts(null);
            this.logger.Debug($"${LoggingPrefix}ProcessInitialization() - Loading contacts results successfull? ${contactsResults.IsSuccess}");
            _contacts = contactsResults.Results;
            this.logger.Debug(_contacts);

            return _contacts; 
        }*/

        public IEnumerable<SynchedPhysicalContacts> LoadPhysicalContacts()
        {
            /*
            this.logger.Debug($"${LoggingPrefix}LoadCategories() - Start");
            _categories = OperationService.UpdateCategories(null).Results;
            this.logger.Debug(_categories);
            this.logger.Debug($"${LoggingPrefix}LoadCategories() - Loading from cache");
            var cache = GetSavedTemporaries<Category>();
            this.logger.Debug($"${LoggingPrefix}LoadCategories() - End");
            return _categories.Union(cache).ToList(); */
            return null;
        }
        private void Init()
        {
            this.logger.Debug($"${LoggingPrefix}Init() - Start - initialized: ${_initialized}");
            if (this._initialized == 1)
                return;

            lock (_sync)
            {
                ProcessInitialization();
            }

            this.logger.Debug($"${LoggingPrefix}Init() - End - initialized: ${_initialized}");
        }

        public void SaveTemporarily<T>(T value) where T: class, IModelDefinition
        {
            this.logger.Debug($"${LoggingPrefix}SaveTemporarily() - Start- value: ");
            this.logger.Debug(value);
            var typeName = typeof(T).Name.ToLowerInvariant();

            if (! _tempCache.ContainsKey(typeName))
            {
                _tempCache.Add(typeName, new List<Object>());
            }

            var list = _tempCache[typeName];
            ClearTemporaryCache(value);

            list.Add(value);
            this.logger.Debug($"${LoggingPrefix}SaveTemporarily() - End");
        }

        public void ClearTemporaryCache<T>(T value) where T : class, IModelDefinition
        {
            this.logger.Debug($"${LoggingPrefix}ClearTemporaryCache() - Start");
            var typeName = typeof(T).Name.ToLowerInvariant();

            if (!_tempCache.ContainsKey(typeName))
            {
                return;
            }

            var list = _tempCache[typeName];

            if (value == null)
            {
                list.Clear();
                return;
            }

            list.RemoveAll(item => {
                T convertedItem = item as T;
                var same = String.Equals(value.Id, convertedItem.Id);
                this.logger.Debug($"${LoggingPrefix}ClearTemporaryCache() - Comparing {value.Id} vs {convertedItem.Id} = {same}");
                return same;
            });

            this.logger.Debug($"${LoggingPrefix}ClearTemporaryCache() - End");
        }

        private IEnumerable<T> GetSavedTemporaries<T>()
        {
            this.logger.Debug($"${LoggingPrefix}GetSavedTemporaries() - Start");
            var list = new List<T>();
            var typeName = typeof(T).Name.ToLowerInvariant();

            if (! _tempCache.TryGetValue(typeName, out List<Object> objList))
            {
                return list;
            }

            foreach(var obj in objList)
            {
                list.Add((T)obj);
            }

            this.logger.Debug($"${LoggingPrefix}GetSavedTemporaries() - End");

            return list;
        }

        public OperationResult<IEnumerable<T>> SaveModels<T>(IEnumerable<Tuple<T, OperationKinds>> modelOperations) where T:  IModelDefinition
        {
            this.logger.Debug($"${LoggingPrefix}SaveModels() - Start");
            var actionableOperations = new List<ActionableOperations<T>>();
            OperationResult<IEnumerable<T>> result;

            foreach (var modelOperation in modelOperations)
            {
                var actionableOperation = new ActionableOperations<T>(modelOperation.Item1, modelOperation.Item2);
                actionableOperations.Add(actionableOperation);
            }
            /*
            if (EqualityComparer<Type>.Default.Equals(typeof(T), typeof(Category)))
            {
                var categoryActionableActions = actionableOperations.Cast<ActionableOperations<Category>>();
                try
                {
                    var response = OperationService.UpdateCategories(categoryActionableActions);
                    result =  new OperationResult<IEnumerable<T>>( response.Results.Cast<T>(), response.ErrorMessage);
                }
                catch(Exception ex)
                {
                    this.logger.Debug($"${LoggingPrefix}SaveModels() - Exception thrown saving categories:  ${ex}");
                    result = new OperationResult<IEnumerable<T>>(null, $"${LoggingPrefix}SaveModels() - Exception thrown saving categories:  ${ex}");
                }
            }
            else if (EqualityComparer<Type>.Default.Equals(typeof(T), typeof(Product)))
            {
                var productActionableActions = actionableOperations.Cast<ActionableOperations<Product>>();
                try
                {
                    var response = OperationService.UpdateProducts(productActionableActions);
                    result = new OperationResult<IEnumerable<T>>(response.Results.Cast<T>(), response.ErrorMessage);
                }
                catch (Exception ex)
                {
                    this.logger.Debug($"${LoggingPrefix}SaveModels() - Exception thrown saving products:  ${ex}");
                    result = new OperationResult<IEnumerable<T>>(null, $"${LoggingPrefix}SaveModels() - Exception thrown saving products:  ${ex}");
                }
            }
            else
            {
                throw new InvalidOperationException("Can't save operation having types of " + typeof(T));
            }

            this.logger.Debug(result);*/
            this.logger.Debug($"${LoggingPrefix}SaveModels() - End");
            return null; // result;
        }

        public Profile LoadProfile()
        { /*
            this.logger.Debug($"${LoggingPrefix}LoadProducts() - Start");
            _products = OperationService.UpdateProducts(null).Results;
            this.logger.Debug(_categories);
            this.logger.Debug($"${LoggingPrefix}LoadProducts() - Loading from cache");
            var cache = GetSavedTemporaries<Product>();
            this.logger.Debug($"${LoggingPrefix}LoadProducts() - End");
            var result =  _products.Union(cache).ToList();
            return result; */
            return null;
        }

        public IOperationsService OperationService { get; protected set; }
    }
}
