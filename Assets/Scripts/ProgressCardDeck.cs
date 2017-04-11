using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ProgressCardDeck
{
	public List<AbstractProgressCard> CurrentDeck;
	public static int CC = 0;

	public static ProgressCardDeck InitialDeck() {
		ProgressCardDeck newDeck = new ProgressCardDeck ();
		newDeck.CurrentDeck = new List<AbstractProgressCard>() {
			//new AlchemistCard(), new AlchemistCard(),
			//new CraneCard(++CC), new CraneCard(++CC),
			//new EngineerCard(++CC),
			//new InventorCard(++CC), new InventorCard(++CC),
			//new IrrigationCard(++CC), new IrrigationCard(++CC),
			//new MedicineCard(++CC), new MedicineCard(++CC),
			//new MiningCard(++CC), new MiningCard(++CC),
			//new PrinterCard(++CC),
			//new RoadBuildingCard(++CC), new RoadBuildingCard(++CC),
			//new SmithCard(++CC), new SmithCard(++CC),
			//new BishopCard(++CC), new BishopCard(++CC),
			new ConstitutionCard(++CC),
			//new DeserterCard(), new DeserterCard(),
			//new DiplomatCard(), new DiplomatCard(),
			//new IntrigueCard(), new IntrigueCard(),
			//new SaboteurCard(), new SaboteurCard(),
			//new SpyCard(), new SpyCard(), new SpyCard(),
			//new WarlordCard(), new WarlordCard(),
			//new WeddingCard(), new WeddingCard(),
			//new CommercialHarborCard(), new CommercialHarborCard(),
			//new MasterMerchantCard(), new MasterMerchantCard(),
			//new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(),
			//new MerchantFleetCard(), new MerchantFleetCard(),
			//new ResourceMonopolyCard(), new ResourceMonopolyCard(), new ResourceMonopolyCard(), new ResourceMonopolyCard(),
			//new TradeMonopolyCard(), new TradeMonopolyCard()
		};

		Random rnd = new Random ();
		newDeck.CurrentDeck = newDeck.CurrentDeck.OrderBy (i => rnd.Next (newDeck.CurrentDeck.Count)).ToList();

		return newDeck;
	}

	public AbstractProgressCard DrawCardOfType(AbstractProgressCard.ProgressCardType type) {
		/*List<AbstractProgressCard> cardsOfType = CurrentDeck.Where (x => x.CardType == type).ToList ();
		if (cardsOfType.Count == 0) {
			return null;
		}

		AbstractProgressCard card = cardsOfType [0];
		//CurrentDeck.Remove (card);*/

		return CurrentDeck[0]; // TODO : fix
	}

	public AbstractProgressCard DrawRandomCard() {
		AbstractProgressCard card = CurrentDeck[UnityEngine.Random.Range (0, CurrentDeck.Count)];
		//CurrentDeck.Remove (card);

		return card;
	}
}
