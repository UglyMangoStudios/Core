# SpaceCore/Data

The contents of this directory pertain to player, company, and global data containers. 

## Contents
#### Discord/[CompanyDiscordData](./Discord/CompanyDiscordData.cs)
Data that is stored within the NoSQL database. This class is the representation of Discord-related data for each company
(e.g. each Discord guild). Guild roles and channels as well as company name, description, etc. are stored here. All members are public.

#### Discord/[PlayerDiscordData](./Discord/PlayerDiscordData.cs)
Data that is stored within the NoSQL database. This class is the representation of Discord-related data for each player. While the player's game data is 
not stored here, the player's residential company jusidiction is referenced here, as well as a few other relative Discord-related data. All members are public.

#### Saves/[PlayerGameData](./Saves/PlayerGameData.cs)
This class stores all of the player's game data and is stored locally due to frequent reading and writing operations. Furthermore, the data within is encapsulated to prevent any 
data quirks. This means to effectively retain all information, this data must be serialized and thusly deseralized across network barriers: simple JSON serialization does not and cannot work. 
See [BinaryService](../Services/BinaryService.cs) that handles this beautifully <3