using DTOs;

namespace ListOfGames
{
    public static class GameList
    {
        public static List<GameDTO> List()
        {
            return new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Name = "Echoes of Aether",
                    Category = "Action RPG",
                    Description =
                        "Across the shattered realms of Aether, ancient rifts pulse with unstable energy and long-forgotten " +
                        "constructs stagger back to life. As a Seeker, you traverse wild frontiers, forge alliances with " +
                        "nomadic guilds, and uncover fractured memories that echo from ages past. Your choices shape the " +
                        "landscapes—rewriting trade routes, restoring relic cities, or unleashing catastrophic storms that " +
                        "forever alter the horizon. Whether you master the harmonics of rift songs or invent clever tools " +
                        "that stitch reality back together, the fate of Aether responds to your courage and curiosity."
                },
                new GameDTO
                {
                    Id = 2,
                    Name = "Nebula Cartographer",
                    Category = "Exploration",
                    Description =
                        "Chart the unspoken silence between stars as a lone cartographer aboard a modest research vessel. " +
                        "Scan crystalline anomalies, decode biosignatures that drift through ion wakes, and weave a living " +
                        "atlas for future travelers. Your maps influence diplomatic corridors, mining rights, and migration " +
                        "paths across a delicate interstellar balance. Every discovery is a thread: tug too hard and colonies " +
                        "strain; weave with care and cultures flourish. Upgrade your ship with modular scanners, customize " +
                        "probe arrays, and curate a record that outlives any single voyage."
                },
                new GameDTO
                {
                    Id = 3,
                    Name = "Harbor of Clockwork Tides",
                    Category = "Simulation",
                    Description =
                        "In a seaside city ruled by the rhythm of brass engines and tidal gears, you manage a bustling harbor " +
                        "where every shipment, schedule, and steam valve matters. Balance the delicate economy as airships " +
                        "dock with exotic cargo and clockwork automatons demand maintenance. Craft contracts, train crews, " +
                        "and modernize piers while preserving the heritage that draws patrons from distant empires. As storms " +
                        "roll in and markets shift, your foresight keeps the turbines humming and the community resilient—" +
                        "a testament to grit, grace, and ingenious design."
                }
            };
        }
    }
}

