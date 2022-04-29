using System.Collections.Generic;

namespace Lavos.Services.Data
{
    public interface IDataSaver
    {
        bool IsDirty { get; }
        string DataFile { get; }

        void WriteData(Dictionary<string, object> data);
    }
}