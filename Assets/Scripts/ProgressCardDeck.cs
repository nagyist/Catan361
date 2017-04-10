using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProgressCardDeck
{
	public List<AbstractProgressCard> CurrentDeck;

	public static ProgressCardDeck InitialDeck() {
		ProgressCardDeck newDeck = new ProgressCardDeck ();
		newDeck.CurrentDeck = new List<AbstractProgressCard>() {
			new AlchemistCard(), new AlchemistCard(),
			new CraneCard(), new CraneCard(),
			new EngineerCard(),
			new InventorCard(), new InventorCard(),
			new IrrigationCard(), new IrrigationCard(),
			new MedicineCard(), new MedicineCard(),
			new MiningCard(), new MiningCard(),
			new PrinterCard(),
			new RoadBuildingCard(), new RoadBuildingCard(),
			new SmithCard(), new SmithCard(),
			new BishopCard(), new BishopCard(),
			new ConstitutionCard(),
			new DeserterCard(), new DeserterCard(),
			new DiplomatCard(), new DiplomatCard(),
			new IntrigueCard(), new IntrigueCard(),
			new SaboteurCard(), new SaboteurCard(),
			new SpyCard(), new SpyCard(), new SpyCard(),
			new WarlordCard(), new WarlordCard(),
			new WeddingCard(), new WeddingCard(),
			new CommercialHarborCard(), new CommercialHarborCard(),
			new MasterMerchantCard(), new MasterMerchantCard(),
			new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(), new MerchantCard(),
			new MerchantFleetCard(), new MerchantFleetCard(),
			new ResourceMonopolyCard(), new ResourceMonopolyCard(), new ResourceMonopolyCard(), new ResourceMonopolyCard(),
			new TradeMonopolyCard(), new TradeMonopolyCard()
		};

		Random rnd = new Random ();
		newDeck.CurrentDeck = newDeck.CurrentDeck.OrderBy (i => rnd.Next (newDeck.CurrentDeck.Count)).ToList();

		return newDeck;
	}

	public AbstractProgressCard DrawCardOfType(AbstractProgressCard.ProgressCardType type) {
		List<AbstractProgressCard> cardsOfType = CurrentDeck.Where (x => x.CardType == type).ToList ();
		if (cardsOfType.Count == 0) {
			return null;
		}

		return cardsOfType[0];
	}
}
