using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Abner.Extensions.Configuration.Db
{
    internal class DefaultDbProvider : IDbProvider, IDisposable
    {
        private ConfigurationReloadToken _reloadToken;
        private bool _isDisposed;

        public DefaultDbProvider(IList<IConfigurationSource> sources)
        {
            foreach (var source in sources)
            {
                if (typeof(DbConfigurationSource).IsAssignableFrom(source.GetType()))
                {
                    var configurationSource = (DbConfigurationSource)source;

                    if (configurationSource.Option.ReloadOnChange)
                    {
                        Task.Run(() =>
                        {
                            while (!_isDisposed)
                            {
                                Task.Delay(configurationSource.Option.ReloadDelay).Wait();
                                if (_reloadToken != null)
                                {
                                    _reloadToken.OnReload();
                                }
                            }
                        });
                    }
                }
            }


        }

        public void Dispose()
        {
            _isDisposed = true;
        }

        public IChangeToken Watch(string tableName)
        {
            _reloadToken = new ConfigurationReloadToken();
            return _reloadToken;
        }

    }
}