using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Challenge.Database.Repositories
{
    public abstract class BaseRepository : IDisposable
    {

        private IDbConnection _dbConnection;
        private bool disposedValue;

        protected BaseRepository(IDbConnection dbConnection)
        {   
            _dbConnection = dbConnection;
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if(_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
                    _dbConnection.Close();

                _dbConnection = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~BaseRepository()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
