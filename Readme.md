# Solveur de labyrinthe

## Introduction

Ce petit projet tente de résoudre les labyrinthe proposés.
En l'état actuel, s'il existe une solution, le logiciel la trouvera.
Cependant si plusieurs solutions sont possibles, il peut ne pas soumettre la plus efficace (le chemin le plus court) à tous les coups.


## Fonctionnalités

- Lit un fichier TXT contenant une matrice de chiffre
- Visualisation graphique de la solution


## Fichier texte

Chaque fichier contient un labyrinthe ; celui-ci se présente sous la forme d'une matrice (ou tableau)
de chiffres dessinant le labyrinthe. Celui-ci doit être aussi long que large.
Le départ, l'arrivée et les murs peuvent être placés n'importe où dans l'enceinte du labyrinthe.

Comment construire le labyrinthe :
- 0 : passage possible
- 1 : mur
- 2 : départ
- 3 : arrivée
- 4 (interne au logiciel) : chemin déjà emprunté par l'algorithme


## Détails techniques du projet
- .Net 6.0
- Visual Studio 2022


## Comment utiliser
- Cloner ou télécharger ce projet
- Ouvrir et générer Labyrinthe
