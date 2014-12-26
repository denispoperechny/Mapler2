using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace DataPersistance.Facade.MockData
{
    public interface IMockDataProvider
    {
        void AddDataCollection<T>(List<T> data) where T : IPersistentModel;
    }
}
