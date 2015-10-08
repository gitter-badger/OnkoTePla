using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler
{
	public class ReplaceAppointmentCommandHandler : IDomainCommandHandler<ReplaceAppointment>
	{
		private readonly IAggregateRepository repository;

		public ReplaceAppointmentCommandHandler (IAggregateRepository repository)
		{
			this.repository = repository;
		}

		private static ActionTag GetDividedActionTag(ActionTag actionTag)
		{
			switch (actionTag)
			{
				case ActionTag.NormalAction: { return ActionTag.NormalDividedReplaceAction; }
				case ActionTag.RedoAction:   { return ActionTag.RedoDividedReplaceAction;   }
				case ActionTag.UndoAction:   { return ActionTag.UndoDividedReplaceAction;   }
			}

			throw new ArgumentException("internal parameter error");
		}

		public void Process(ReplaceAppointment command)
		{
			if (command.NewDate == command.OriginalDate)
			{
				var aggregate = repository.GetById(command.SourceAggregateId);

				aggregate.ReplaceAppointemnt(command.UserId,
											 command.SourceAggregateVersion,
											 command.PatientId,
											 command.ActionTag,
											 command.NewDescription,
											 command.NewDate,
											 command.NewStartTime,
											 command.NewEndTime,
											 command.NewTherapyPlaceId,
											 command.OriginalAppointmendId);

				repository.Save(aggregate);
			}
			else
			{
				var sourceAggregate      = repository.GetById(command.SourceAggregateId);
				var destinationAggregate = repository.GetById(command.DestinationAggregateId);

				sourceAggregate.DeleteAppointment(command.UserId, 
												  command.SourceAggregateVersion, 
												  command.UserId, 
												  GetDividedActionTag(command.ActionTag),
												  command.OriginalAppointmendId);

				destinationAggregate.AddAppointment(command.UserId, 
												    command.DestinationAggregateVersion,
													GetDividedActionTag(command.ActionTag),
													new CreateAppointmentData(command.PatientId, 
																			  command.NewDescription, 
																			  command.NewStartTime, 
																			  command.NewEndTime, 
																			  command.NewDate, 
																			  command.NewTherapyPlaceId, 
																			  command.OriginalAppointmendId));
				repository.Save(sourceAggregate);
				repository.Save(destinationAggregate);
			}
		}
	}
}
