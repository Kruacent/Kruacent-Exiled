using Exiled.API.Enums;
using PlayerRoles;

namespace KruacentExiled.CustomRoles.CustomSCPTeam
{
    public class EndRoundClassList
    {
        public int scp;
        public int chaos;
        public int mtf;
        public int flamingo;

        public EndRoundClassList()
        {
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {

                if (SCPTeam.IsSCP(allHub))
                {
                    scp++;
                    continue;
                }

                switch (allHub.GetTeam())
                {
                    case Team.ClassD:
                    case Team.ChaosInsurgency:
                        chaos++;
                        break;
                    case Team.FoundationForces:
                    case Team.Scientists:
                        mtf++;
                        break;
                    case Team.Flamingos:
                        flamingo++;
                        break;
                }
            }
        }


        public bool CanRoundEnd(out LeadingTeam leadingTeam)
        {
            int nbFactionAlive = 0;
            leadingTeam = LeadingTeam.Draw;

            if (flamingo > 0)
            {
                nbFactionAlive++;
                leadingTeam = LeadingTeam.Flamingo;
            }
            if (chaos > 0)
            {
                nbFactionAlive++;
                leadingTeam = LeadingTeam.ChaosInsurgency;
            }
            
            
            if (mtf > 0)
            {
                nbFactionAlive++;
                leadingTeam = LeadingTeam.FacilityForces;
            }

            if (scp > 0)
            {
                nbFactionAlive++;
                leadingTeam = LeadingTeam.Anomalies;
            }

            return nbFactionAlive <= 1;

        }
    }
}
