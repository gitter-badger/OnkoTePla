namespace bytePassion.OnkoTePla.Contracts.Domain.Events.Base
{
	public enum ActionTag
    {
		RegularAction,		
		UndoAction,
		RedoAction,
		
		RegularDividedReplaceAction,
		UndoDividedReplaceAction,
		RedoDividedReplaceAction
	}
}
