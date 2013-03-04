Card JSON Description:
This will explain the notaion used in the card.json file for:
	name
	image
	players
	guild
	cost
	effects
	chains

NOTE: The first case is the orginal, just to compare what the options are for the following images
	28-40 Victory points
	58-59 Coins
	60-63 Coins/Victory
	
CARDS:
	{
		"name": 	"<Card Name>"
		"image": 	"<Card Image Name>"
		"players":	<Amount of minimum players required for this card to be used>
		"age":		<Which deck AGE this card will appear in>
		"guild":	"<colour of the structure/card>"
		"cost":		{<cost of the structure>, ..., <>}
		"effects":	[{<Array of effects the cards have when built>}, ..., {}]
		"chains":	["<Array of strings of the previous structures needed in order to build this card>"]
		
	}
=========================================================================================================	
NAME:
	Should be a string

=========================================================================================================	
IMAGE:
	Should be a string without the .jpg
	<name>_<age>_<players>

=========================================================================================================	
PLAYERS:
	Should be an integer ranging from 3-6
	
=========================================================================================================
AGE:
	Should be and integer ranging from 0-3, 0 will be used for guild cards

=========================================================================================================	
GUILD:
	brown
	gray
	blue
	green
	yellow
	red
	purple

=========================================================================================================	
COST:
	Gives a object with resource or coin cost along with its value for the amount it needs.	
	Can range from raw resources to manufactured resources and coin
		c	clay
		o	ore
		s	stone
		w	wood
		g	glass
		l	loom
		p	papyrus
		coin
		
	
	NOTE: I believe it should be in object notation where:
	
		Objects
			{ string : value }, { string : value }, ...
			
		All contained within an array maybe.
		
=========================================================================================================
EFFECTS:
	[Army]
		{
			"army": <amount>
		}
		
		Gives the number of 'army' to the players currently military power
	
	
	[Raw Resource Cost Trading via East]
		{
			"rcostEast":1
		}
		
		Instead of the player purchasing resources from the player to the East (right) for
		2 coins, it is reduced to 1.
		
		NOTE: 	The 'r' in the rcostEast dictates that only raw resources can only be traded for 1 coin.
				Manufacted resources such as glass, loom, or papyrus is not included within this category.


	[Raw Resource Cost Trading via West]
		{
			"rcostWest": 1
		}
		
		Instead of the player purchasing resources from the player to the West (left) for
		2 coins, it is reduced to 1.
		
		NOTE: 	The 'r' in the rcostWest dictates that only raw resources can only be traded for 1 coin.
				Manufacted resources such as glass, loom, or papyrus is not included within this category.
			

	[Manufactured Resource Cost Trading via East]
		{		
			"mcostEast":1
		}

		Similar to Raw Resource trading of East


	[Manufactured Resource Cost Trading via West]
		{		
			"mcostWest":1
		}

		Similar to Raw Resource trading of West



	[Resource Production]
		{
			"<resource letter>" <amount>
		}
			
		Can range from raw resources to manufactured resources
			c	clay
			o	ore
			s	stone
			w	wood
			g	glass
			l	loom
			p	papyrus
			
			
	[Resource Choice]
		{
			"rchoice": [<array of choices>, ..., <choice n>]
		}
		
		Can range from raw resources to manufactured resources
		c	clay
		o	ore
		s	stone
		w	wood
		g	glass
		l	loom
		p	papyrus
		
	
	[Science]
		{
			"science": "<science letter>"
		}
		
		Gives the player a scientific symbol determined by the letter
		tab		tablet
		comp	compass
		gear	gear

	
	[Science Choice]
		{
			"schoice": ["<array of science choices>"]
		}
		
		tab		tablet
		comp	compass
		gear	gear
		
		
	[Special Effects of Cards]
		{
			"type":		"<Parameter being gained>",
			"amount":	<The amount recieved>,
			"from": 	"<Who>",
			"basis": 	"<Determinant on what is the factor of gaining the type and amount>"
			"list": 	[c1,c2,...,cn]
		}

		TYPE:
			coin		Coins given via the amount depending on the basis from whom
			victory		Victory points given via the amount depending on the masis from whom
			lastcard	Play last card and everything else is empty
			
		AMOUNT:
			##			The number of types that will be given to the player, usually from 1-8

		FROM:
			player 		The player
			neighbors 	Both the West and East neighbors of the player
			west 		West neighbor
			east 		East neighbor
			all		Player + neigbors
			none		Nothing is taken into account

		BASIS:
			wonderstages	Number of wonders built, dependant on who <FROM>
			
							Can we shortenthis to just 'wonders' because it  
							can be clearly seen its about number of stages
			
			none			Nothing is taken into account
							
			
			
			brown			Number of 'brown' structures built
			gray			Number of 'gray' structures built
			blue			Number of 'blue' structures built
			green			Number of 'green' structures built
			yellow			Number of 'yellow' structures built
			red				Number of 'red' structures built
			purple			Number of 'purple' structures built

=========================================================================================================
CHAINS:
	["<Card Name>", ..., "<Card n>"]

	An array of strings of structure names that should be built in the previous age to build the current
	structure for free.
	