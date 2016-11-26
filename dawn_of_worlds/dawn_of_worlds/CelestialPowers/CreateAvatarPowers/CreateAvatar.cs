using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.CelestialPowers.CommandAvatarPowers;
using dawn_of_worlds.CelestialPowers.CommandCityPowers;
using dawn_of_worlds.CelestialPowers.CommandRacePowers;
using dawn_of_worlds.CelestialPowers.CommandNationPowers;
using dawn_of_worlds.CelestialPowers.CreateOrderPowers;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CreateAvatarPowers
{
    class CreateAvatar : Power
    {
        private AvatarType _type { get; set; }
        private Race _race { get; set; }
        private Nation _nation { get; set; }
        private Order _order { get; set; }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Avatar created_avatar = new Avatar("PlaceHolder", creator);

            created_avatar.Type = _type;
            created_avatar.AvatarRace = _race;

            created_avatar.MasterNation = _nation;
            if (created_avatar.MasterNation != null)
                created_avatar.MasterNation.Subjects.Add(created_avatar);

            created_avatar.OrderMembership = _order;
            if (created_avatar.OrderMembership != null)
                created_avatar.OrderMembership.Members.Add(created_avatar);     

            switch (created_avatar.Type)
            {
                case AvatarType.Champion: 
                    break;
                case AvatarType.General:
                    break;
                case AvatarType.HighPriest:
                    if (_order != null)
                    {
                        created_avatar.OrderMembership.Leader = created_avatar;
                    }
                    else if (_nation != null)
                    {
                        creator.Powers.Add(new UsePower(created_avatar, new CreateOrder(OrderType.Church, OrderPurpose.FounderWorship, _nation, null)));
                    }                    
                    break;
                case AvatarType.LegendaryBeast:
                    if (_nation != null)
                    {
                        created_avatar.MasterNation.Leader = created_avatar;
                    }
                    break;
                case AvatarType.RoyalDynasty:
                    if (_nation != null)
                    {
                        created_avatar.MasterNation.Leader = created_avatar;
                    }                
                    break;
            }

            creator.Powers.Add(new UsePower(created_avatar, new FoundNation(created_avatar.AvatarRace)));

            created_avatar.Name = created_avatar.AvatarRace.Name + " " + created_avatar.Type.ToString();

            if (_nation != null)
            {
                creator.Powers.Add(new UsePower(created_avatar, new CreateCity(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new ExpandTerritory(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new DeclareWar(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new FormAlliance(created_avatar.MasterNation)));

                foreach (City city in created_avatar.MasterNation.Cities)
                {
                    creator.Powers.Add(new UsePower(created_avatar, new RaiseArmy(city)));
                }
            }

            creator.CreatedAvatars.Add(created_avatar);

            creator.LastCreation = created_avatar;
        }

        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 7;
                case 3:
                    return 8;
                default:
                    return 1;
            }
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = 0;

            switch (current_age)
            {
                case 1:
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case 2:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                case 3:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
                default:
                    weight += 0;
                    break;
            }

            int cost = Cost(current_age);
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;


            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_type == AvatarType.LegendaryBeast && _race.Type == SpeciesType.Dragonoid)
                weight += Constants.WEIGHT_STANDARD_CHANGE * 2;

            if (_type == AvatarType.LegendaryBeast && _race.Type == SpeciesType.Humanoid)
                weight -= Constants.WEIGHT_STANDARD_CHANGE * 2;

            return weight >= 0 ? weight : 0;
        }



        public CreateAvatar(AvatarType type, Race race, Nation nation, Order order)
        {
            Name = "Create Avatar: " + type + "|" + (race != null ? race.ToString() : "") + "|" + (nation != null ? nation.ToString() : "") + "|" + (order != null ? order.ToString() : "");
            _type = type;
            _race = race;
            _nation = nation;
            _order = order;
        }
    }
}
