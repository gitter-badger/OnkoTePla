using System;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public interface IStreamMetaDataService
    {
        PracticeMetaData ReadMetaDataForPractice(Guid practiceId);
        void SaveMetaData(Guid practiceId, PracticeMetaData data);
    }
}