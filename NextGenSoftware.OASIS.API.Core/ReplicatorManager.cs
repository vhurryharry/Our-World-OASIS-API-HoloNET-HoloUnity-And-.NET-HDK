﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NextGenSoftware.OASIS.API.Core.ProfileManager;

namespace NextGenSoftware.OASIS.API.Core
{
    public class ReplicatorManager : OASISManager
    {
       // private ProfileManagerConfig _config;

        public List<IOASISStorage> OASISStorageProviders { get; set; }

        //public Task<IProfile> LoadProfileAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public ProfileManagerConfig Config
        //{
        //    get
        //    {
        //        if (_config == null)
        //        {
        //            _config = new ProfileManagerConfig();
        //        }

        //        return _config;
        //    }
        //}




        //RegisterDatabaseForReplication(string connectionString, string dbName);
        //RegisterTableForReplication(string connectionString, string dbName, string tableName);
        //RegisterTablesForReplication(string connectionString, string dbName, string<> tables);
        //RegisterQueryForReplication(string connectionString, string dbName, string query);

        //Events
        public delegate void ReplicatorManagerError(object sender, ProfileManagerErrorEventArgs e);
        public event ProfileManagerError OnProfileManagerError;

        public delegate void StorageProviderError(object sender, ProfileManagerErrorEventArgs e);

        /*
        public ProfileManager(List<IOASISStorage> OASISStorageProviders)
        {
            this.OASISStorageProviders = OASISStorageProviders;

            foreach (IOASISStorage provider in OASISStorageProviders)
            {
                provider.OnStorageProviderError += OASISStorageProvider_OnStorageProviderError;
                provider.ActivateProvider();
            }
        }*/

       //TODO: In future more than one storage provider can be active at a time where each call can specify which provider to use.
        public ReplicatorManager(IOASISStorage OASISStorageProvider) : base(OASISStorageProvider)
        {
            //if (!ProviderManager.IsProviderRegistered(OASISStorageProvider))
            //    ProviderManager.RegisterProvider(OASISStorageProvider);

            //ProviderManager.SwitchCurrentStorageProvider(OASISStorageProvider.ProviderType);

         //   return null;
        }

        private void OASISStorageProvider_OnStorageProviderError(object sender, ProfileManagerErrorEventArgs e)
        {
            //TODO: Not sure if we need to have a OnProfileManagerError as well as the StorageProvider Error Event?
            OnProfileManagerError?.Invoke(this, e);
        }

        public async Task<IProfile> LoadProfileAsync(string providerKey, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).LoadProfileAsync(providerKey);

            return await ProviderManager.CurrentStorageProvider.LoadProfileAsync(providerKey);
        }

        public async Task<IProfile> LoadProfileAsync(Guid id, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).LoadProfileAsync(id);

            return await ProviderManager.CurrentStorageProvider.LoadProfileAsync(id);
        }

        public async Task<IProfile> LoadProfileAsync(string username, string password, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).LoadProfileAsync(username, password);

            return await ProviderManager.CurrentStorageProvider.LoadProfileAsync(username, password);
        }

        public async Task<IProfile> SaveProfileAsync(IProfile profile, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).SaveProfileAsync(profile);

            return await ProviderManager.CurrentStorageProvider.SaveProfileAsync(profile);
        }

        public async Task<KarmaAkashicRecord> AddKarmaToProfileAsync(IProfile profile, KarmaTypePositive karmaType, KarmaSourceType karmaSourceType, string karamSourceTitle, string karmaSourceDesc, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).AddKarmaToProfileAsync(profile, karmaType, karmaSourceType, karamSourceTitle, karmaSourceDesc);

            return await ProviderManager.CurrentStorageProvider.AddKarmaToProfileAsync(profile, karmaType, karmaSourceType, karamSourceTitle, karmaSourceDesc);
        }

        public async Task<KarmaAkashicRecord> RemoveKarmaFromProfileAsync(IProfile profile, KarmaTypeNegative karmaType, KarmaSourceType karmaSourceType, string karamSourceTitle, string karmaSourceDesc, ProviderType provider = ProviderType.Default)
        {
            if (provider != ProviderType.Default)
                return await ((IOASISStorage)ProviderManager.GetAndActivateProvider(provider)).SubtractKarmaFromProfileAsync(profile, karmaType, karmaSourceType, karamSourceTitle, karmaSourceDesc);

            return await ProviderManager.CurrentStorageProvider.SubtractKarmaFromProfileAsync(profile, karmaType, karmaSourceType, karamSourceTitle, karmaSourceDesc);
        }
    }
}
