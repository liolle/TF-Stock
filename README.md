# Mini labo de dev
Réaliser une application de gestion de stock pour un entrepot.

## Objectif
Mettre en place une application Web qui permet à des utilisateurs d'encoder des entrées et sorties de stock de produit.

### L'application doit permettre aux utilisateurs de : 
- Consulter les stocks des différents produits sous la forme d'une liste
- Consulter l'historique des transactions d'un stock de produits
- Afficher un graphique avec le stock

### L'application permet également de réaliser l'inventaire des produits
- Dashboard pour consulter le stock des produits à l'aide d'un tableau avec les données suivantes : 
  - Nom du produit
  - Nombre de transactions
  - Date de dernière transaction
  - Nombre de produits en stock
- Il peut corriger le stock d'un produit, si celui-ci ne correspond pas à la réalité (Vol ?).

## Règles métier
- Une entrée ou sortie de stock ne peut pas être négative ou nulle.
- Un stock ne peut pas passer en négatif.
- Les transactions sont limitées à un maximum de 1 000 produits.
- Le prix des produits ne peut pas être négatif.

## Contraintes
 - Application ASP.Net MVC
 - Pattern CQS
 - Utilisation d'ADO (Avec possibilité d'installer Dapper)

## Structure de données

### Utilisateur de l'app
- Login
- Password
- Email
- Prénom
- Nom

### Produit
- Nom
- Marque
- GTIN [_(Global Trade Item Number)_](https://fr.wikipedia.org/wiki/Global_Trade_Item_Number)
- Prix

### Transaction de stock
- Produit
- Quantité
- Type (Entrée, Sortie, Inventaire)
- Utilisateur
- Date de la transaction


**NB : Le client _(cf: le formateur quoi)_ a la possibilité d'ajouter ou de modifier la demande à tout moment.**
