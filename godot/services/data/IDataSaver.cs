using System.Collections.Generic;

namespace Lavos.Services.Data
{
    public interface IDataSaver
    {
        bool IsDirty { get; }
        string DataFile { get; }

        void LoadData(Dictionary<string, string> data);
        void WriteData(Dictionary<string, string> data);
    }
}