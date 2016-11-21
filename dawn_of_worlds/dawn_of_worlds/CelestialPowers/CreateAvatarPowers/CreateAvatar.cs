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
            Avatar created_avatar = new Avatar("Master Guy", creator);

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
                        creator.Powers.Add(new UsePower(created_avatar, new CreateOrder(OrderType.Church, OrderPurpose.WorshipFounder, _nation, null)));
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

            if (_nation != null)
            {
                creator.Powers.Add(new UsePower(created_avatar, new CreateCity(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new DeclareWar(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new FormAlliance(created_avatar.MasterNation)));

                foreach (City city in created_avatar.MasterNation.Cities)
                {
                    creator.Powers.Add(new UsePower(created_avatar, new RaiseArmy(city)));
                    creator.Powers.Add(new UsePower(created_avatar, new ExpandCityInfluence(city)));
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
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 100;
                case 3:
                    return 50;
                default:
                    return 100;
            }
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
