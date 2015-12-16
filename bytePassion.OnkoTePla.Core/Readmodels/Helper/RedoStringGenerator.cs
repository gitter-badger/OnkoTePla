using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels.Helper
{
    public static class RedoStringGenerator
    {
        public static string ForAddedEvent(Patient patient, Date day, Time startTime, Time endTime)
        {
            return "Wollen Sie den, durch UNDO entfertnen Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"am {day}\n" +
                  $"von {startTime} bis {endTime}\n" +
                   "wieder herstellen?";
        }

        public static string ForDeletedEvent(Patient patient, Date day, Time currentStartTime, Time currentEndTime)
        {
            return "Wollen Sie den, durch UNDO wieder hinzugefügten Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"vom {day} "+ 
                  $"von {currentStartTime} bis {currentEndTime}]\n" +
                   "doch entfernen?";
        }

        public static string ForDividedReplacedEvent(Patient patient,
                                                     Date currentDate,      Date nextDate,
                                                     Time currentStartTime, Time nextStartTime,
                                                     Time currentEndTime,   Time nextEndTime,
                                                     TherapyPlace currentTherapyPlace, TherapyPlace nextTherapyPlace)
        {
            return "Wollen Sie den, durch UNDO verschobenen Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"vom {currentDate} [{currentStartTime} bis {currentEndTime}; Therapieplatz {currentTherapyPlace.Name}]\n" +
                  $"zum {nextDate} [{nextStartTime} bis {nextEndTime}; Therapieplatz {nextTherapyPlace.Name}]\n" +
                   "zurückverschieben?";
        }
        
        public static string ForReplacedEvent(Patient patient, Date day,
                                              Time currentStartTime, Time currentEndTime, TherapyPlace currentTherapyPlace,
                                              Time nextStartTime,    Time nextEndTime,    TherapyPlace nextTherapyPlace)
        {
            if (currentTherapyPlace == nextTherapyPlace && currentStartTime == nextStartTime)
            {
                return "Wollen Sie das, durch UNDO verschobenes Terminende\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentEndTime}\n" +
                      $"nach {nextEndTime}\n" +
                      "zurückverschieben?";
            }

            if (currentTherapyPlace == nextTherapyPlace && currentEndTime == nextEndTime)
            {
                return "Wollen Sie den, durch UNDO verschobenen Terminbegin\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentStartTime}\n" +
                      $"nach {nextStartTime}\n" +
                      "zurückverschieben?";
            }

            var fromExtension = "";
            var toExtension = "";

            if (currentTherapyPlace != nextTherapyPlace)
            {
                fromExtension = $"\t[Therapieplatz {currentTherapyPlace.Name}]";
                toExtension   = $"\t[Therapieplatz {nextTherapyPlace.Name}]";
            }

            if (new Duration(currentStartTime, currentEndTime) == new Duration(nextStartTime, nextEndTime))
            {
                return "Wollen Sie den, durch UNDO verschobenen Termin\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentStartTime} " + fromExtension + "\n" +
                      $"nach {nextStartTime}" + toExtension + "\n" +
                      "zurückverschieben?";
            }

            return "Wollen Sie den, durch UNDO verschobenen Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"am {day}\n" +
                  $"von {currentStartTime} bis {currentEndTime} " + fromExtension + "\n" +
                  $"nach {nextStartTime} bis {nextEndTime} " + toExtension + "\n" +
                  "zurückverschieben?";
        }
    }
}
