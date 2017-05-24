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
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CreateAvatarPowers
{
    class CreateAvatar : Power
    {
        private AvatarType _type { get; set; }
        private Race _race { get; set; }
        private Civilisation _nation { get; set; }
        private Order _order { get; set; }

        protected override void initialize()
        {
            Name = "Create Avatar: " + _type + "|" + (_race != null ? _race.ToString() : "") + "|" + (_nation != null ? _nation.ToString() : "") + "|" + (_order != null ? _order.ToString() : "");
            BaseCost = new int[] { 10, 7, 8 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_MEDIUM, Constants.WEIGHT_STANDARD_HIGH };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;

            Tags = new List<CreationTag>() { CreationTag.Creation };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            return true;
        }

        public override int Effect(Deity creator)
        {
            Avatar created_avatar = new Avatar("PlaceHolder", creator);

            created_avatar.Type = _type;
            created_avatar.AvatarRace = _race;

            created_avatar.MasterNation = _nation;
            if (created_avatar.MasterNation != null)
                created_avatar.MasterNation.Avatars.Add(created_avatar);

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

            
            //creator.Powers.Add(new UsePower(created_avatar, new FoundNation(created_avatar.AvatarRace, PolityDefinitions.BandSociety)));

            created_avatar.Name.Singular = created_avatar.AvatarRace.Name + " " + created_avatar.Type.ToString();

            if (_nation != null && !_nation.isDestroyed)
            {
                creator.Powers.Add(new UsePower(created_avatar, new CreateCity(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new ExpandTerritory(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new EstablishContact(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new DeclareWar(created_avatar.MasterNation)));
                creator.Powers.Add(new UsePower(created_avatar, new FormAlliance(created_avatar.MasterNation)));

                foreach (City city in created_avatar.MasterNation.Cities)
                {
                    creator.Powers.Add(new UsePower(created_avatar, new RaiseArmy(city)));
                }
            }

            creator.CreatedAvatars.Add(created_avatar);

            creator.LastCreation = created_avatar;

            return 0;
        }

        public CreateAvatar(AvatarType type, Race race, Civilisation nation, Order order)
        {

            _type = type;
            _race = race;
            _nation = nation;
            _order = order;

            initialize();
        }
    }
}
