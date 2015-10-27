﻿using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels.Helper
{
    public static class UndoStringGenerator
    {
        public static string ForAddedEvent(Patient patient, Date day, Time startTime, Time endTime)
        {
            return "Wollen Sie den neu erstellten Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"der momentan am {day}\n" +
                  $"von {startTime} bis {endTime}\n" +
                   "besteht wieder löschen?";
        }

        public static string ForDeletedEvent(Patient patient, Date day, Time lastStartTime, Time lastEndTime)
        {
            return "Wollen Sie den gelöschten Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"vom {day} [{lastStartTime} bis {lastEndTime}]\n" +
                   "wieder herstellen?";
        }
        
        public static string ForDividedReplacedEvent(Patient patient,
                                                     Date currentDate,     Date lastDate,
                                                     Time currentStartTime,Time lastStartTime,
                                                     Time currentEndTime,  Time lastEndTime,
                                                     TherapyPlace currentTherapyPlace,TherapyPlace lastTherapyPlace)
        {
            return "Wollen Sie den Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"vom {lastDate} [{lastStartTime} bis {lastEndTime}; Therapieplatz {lastTherapyPlace.Name}]\n" +
                  $"zum {currentDate} [{currentStartTime} bis {currentEndTime}; Therapieplatz {currentTherapyPlace.Name}]\n" +
                   "verschieben?";
        }

        public static string ForReplacedEvent(Patient patient, Date day,
                                              Time currentStartTime, Time currentEndTime, TherapyPlace currentTherapyPlace,
                                              Time lastStartTime,    Time lastEndTime,    TherapyPlace lastTherapyPlaceType)
        {
            if (currentTherapyPlace == lastTherapyPlaceType && currentStartTime == lastStartTime)
            {
                return "Wollen Sie das Terminende\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentEndTime}\n" +
                      $"nach {lastEndTime}\n" +
                      "verschieben?";
            }

            if (currentTherapyPlace == lastTherapyPlaceType && currentEndTime == lastEndTime)
            {
                return "Wollen Sie den Terminbegin\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentStartTime}\n" +
                      $"nach {lastStartTime}\n" +
                      "verschieben?";
            }

            var fromExtension = "";
            var toExtension = "";

            if (currentTherapyPlace != lastTherapyPlaceType)
            {
                fromExtension = $"[Therapieplatz {currentTherapyPlace.Name}]";
                toExtension   = $"[Therapieplatz {lastTherapyPlaceType.Name}]";
            }

            if (new Duration(currentStartTime, currentEndTime) == new Duration(lastStartTime, lastEndTime))
            {
                return "Wollen Sie den Termin\n" +
                      $"des Patienten {patient.Name}\n" +
                      $"am {day}\n" +
                      $"von {currentStartTime} " + fromExtension + "\n" +
                      $"nach {lastStartTime}" + toExtension + "\n" +
                      "verschieben?";
            }

            return "Wollen Sie den Termin\n" +
                  $"des Patienten {patient.Name}\n" +
                  $"am {day}\n" +
                  $"von {currentStartTime} bis {currentEndTime} " + fromExtension + "\n" +
                  $"nach {lastStartTime} bis {lastEndTime} " + toExtension + "\n" +
                  "verschieben?";
        }
    }
}
