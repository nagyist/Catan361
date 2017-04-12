using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ProgressCardDeck
{
	public List<AbstractProgressCard> createdCards;
	public List<AbstractProgressCard> CurrentDeck;
	public static int CC = 0;

	public static ProgressCardDeck InitialDeck() {
		ProgressCardDeck newDeck = new ProgressCardDeck ();
		newDeck.createdCards = new List<AbstractProgressCard>() {
			//new AlchemistCard(), new AlchemistCard(),
			new CraneCard(++CC), new CraneCard(++CC),
			new EngineerCard(++CC),
			new InventorCard(++CC), new InventorCard(++CC),
			new IrrigationCard(++CC), new IrrigationCard(++CC),
			new MedicineCard(++CC), new MedicineCard(++CC),
			new MiningCard(++CC), new MiningCard(++CC),
			new PrinterCard(++CC),
			new RoadBuildingCard(++CC), new RoadBuildingCard(++CC),
			new SmithCard(++CC), new SmithCard(++CC),
			new BishopCard(++CC), new BishopCard(++CC),
			new ConstitutionCard(++CC),
			new DeserterCard(++CC), new DeserterCard(++CC),
			new DiplomatCard(++CC), new DiplomatCard(++CC),
			new IntrigueCard(++CC), new IntrigueCard(++CC),
			//new SaboteurCard(), new SaboteurCard(),
			new SpyCard(++CC), new SpyCard(++CC), new SpyCard(++CC),
			new WarlordCard(++CC), new WarlordCard(++CC),
			//new WeddingCard(), new WeddingCard(),
			//new CommercialHarborCard(), new CommercialHarborCard(),
			//new MasterMerchantCard(), new MasterMerchantCard(),
			//new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(),
			//new MerchantFleetCard(), new MerchantFleetCard(),
			new ResourceMonopolyCard(++CC), new ResourceMonopolyCard(++CC), new ResourceMonopolyCard(++CC), new ResourceMonopolyCard(++CC),
			//new TradeMonopolyCard(), new TradeMonopolyCard(),
			new DefenderOfCatanProgressCard(++CC), new DefenderOfCatanProgressCard(++CC), new DefenderOfCatanProgressCard(++CC), new DefenderOfCatanProgressCard(++CC), new DefenderOfCatanProgressCard(++CC), new DefenderOfCatanProgressCard(++CC)
		};

		newDeck.CurrentDeck = new List<AbstractProgressCard> ();
		newDeck.CurrentDeck.AddRange (newDeck.createdCards);

		Random rnd = new Random ();
		newDeck.CurrentDeck = newDeck.CurrentDeck.OrderBy (i => rnd.Next (newDeck.CurrentDeck.Count)).ToList();

		return newDeck;
	}

	public AbstractProgressCard DrawCardOfType(AbstractProgressCard.ProgressCardType type) {
		List<AbstractProgressCard> cardsOfType = CurrentDeck.Where (x => x.CardType == type).ToList ();
		if (cardsOfType.Count == 0) {
			return null;
		}

		AbstractProgressCard card = cardsOfType [0];
		CurrentDeck.Remove (card);

		return card;
	}

	public AbstractProgressCard DrawRandomCard() {
		List<AbstractProgressCard> cards = CurrentDeck.Where (x => 
			x.CardType == AbstractProgressCard.ProgressCardType.Politic ||
		                            x.CardType == AbstractProgressCard.ProgressCardType.Science ||
		                            x.CardType == AbstractProgressCard.ProgressCardType.Trade ||
			x.CardType == AbstractProgressCard.ProgressCardType.Barbarian).ToList ();
		if (cards.Count == 0) {
			return null;
		}

		AbstractProgressCard card = cards [UnityEngine.Random.Range(0, cards.Count)];
		CurrentDeck.Remove (card);

		return card;
	}
}
