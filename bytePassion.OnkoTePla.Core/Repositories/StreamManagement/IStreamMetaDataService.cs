using System;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public interface IStreamMetaDataService
    {
        void ReadMetaData(Guid practiceId);
        void SaveMetaData(Guid practiceId);
    }
}