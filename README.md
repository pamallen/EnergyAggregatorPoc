# EnergyAggregatorPoc

Ce Poc se présente sous la forme d'une application C# console.

Elle prend en entrée 3 paramètres : 
- Une date de début ("from", format DD-MM-YYYY)
- Une date de fin ("to", format DD-MM-YYYY)
- Un format de sortie ("format", de type string) (qui n'est pas utilisé... pour le moment)

Un projet de tests unitaires est également présent, vide pour le moment.

Plusieurs classes :

Aggregator.cs : L'aggrégateur. 
- TimeStep, de type TimeSpan: largeur de son intervalle temporel 
- PowerFactories, de type List<PowerFactory> : liste des centrales à aggréger
Le constructeur est privé pour forcer l'utilisation de la méthode Create, où l'on peut placer la logique métier (si besoin est).
Une méthode AddFactory permet d'ajouter des centrales à la liste PowerFactories, en appliquant aussi la logique métier (non implémentée ici).

PowerFactory.cs : Une centrale. 
- ApiUrl, de type String : l'url de l'API à appeler pour récupérer les informations de génération pour la centrale
- ResponseFormat, de type String : le format de la réponse de l'API (non utilisé)
- TimeStep, de type TimeSpan : l'intervalle de temps utilisé pour représenter les informations retournées par la centrale
- EnergyProductions, de type List<EnergyProduction> : la liste des rapports de génération d'énergie, retournés par l'api.
Comme pour l'aggrégateur, la méthode Create permet de gérer la logique métier.
La méthode GetEnergyProductions() permet d'envoyer une requête à l'API de la centrale pour récupérer les rapports de génération d'énergie, et les stock dans EnergyProductions
  
EnergyProduction.cs : Un rapport de génération d'énergie
- StartTime, de type DateTime : le début de l'intervalle de temps du rapport
- EndTime, de type DateTime : la fin de l'intervalle de temps du rapport
- Power, de type double : la quantité d'énergie généree sur l'intervalle


EnergyReading.cs : Une classe permettant de déserialiser les JSON reçus depuis les API

Note : L'agrégateur ne retourne pas les bonnes données.
Il utilise un système de fenêtre glissante (boucle while, avec une date de début et de fin séparée du timespan choisi). Cependant, il récupère pour chaque factory, le EnergyProduction qui contient la fenêtre glissante. Il additionne ensuite pour chacune des factory, la quantité d'énergie produite.
Le problème vient du fait que pour une fenêtre de 15 minutes, par exemple, un reading de 30 minutes va être considéré comme produit 2 fois. En effet, il sera récupéré une première fois, additionné au total pour les minutes 0-15, puis sera à nouveau récupéré pour les minutes 15-30 et additionné à nouveau. Ainsi, la production d'énergie sera retournée au double de ce qui a réellement été produit (pour cet exemple).


Packages utilisés : Newtonsoft.Json pour aider à la sérialisation/désérialisation de JSON.
