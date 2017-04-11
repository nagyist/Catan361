using System;

[Serializable]
public abstract class AbstractProgressCard
{
	public enum ProgressCardType {
		Science,
		Politic,
		Trade
	}

	public ProgressCardType CardType;
	public int Id = 0;

	public AbstractProgressCard (int id)
	{
		this.Id = id;
	}

	public AbstractProgressCard() {
		this.Id = 0;
	}

	public abstract void ExecuteCardEffect();

	public virtual string GetTitle() {
		return "No Title";
	}

	public virtual string GetDescription() {
		return "No Description";
	}

	public void RemoveFromPlayerHand() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdRemoveProgressCard (
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName,
			SerializationUtils.ObjectToByteArray (this)
		);
	}

	public override bool Equals(Object obj) {
		if(obj == null || GetType() != obj.GetType()) {
			return false;
		}

		return ((AbstractProgressCard)obj).Id == this.Id;
	}

	public override int GetHashCode() {
		return this.Id;
	}
}