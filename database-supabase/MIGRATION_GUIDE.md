# GUIDE DE DÉPLOIEMENT SUPABASE & ASP.NET CORE 6 / ANGULAR 15

Ce guide vous détaille comment configurer votre base de données PostgreSQL sur Supabase et y connecter votre API ASP.NET Core 6 et votre application Angular 15.

---

## 1. Configuration de la Base PostgreSQL sur Supabase

1. Créez un projet sur **[Supabase.com](https://supabase.com)** (ex: `batipromo-prod`).
2. Allez dans le menu **SQL Editor** dans la barre latérale de Supabase.
3. Copiez et collez l'intégralité du script `schema_supabase.sql` situé dans ce dossier.
4. Cliquez sur **RUN** pour créer l'ensemble des tables (chantiers, caisses, utilisateurs, stocks, etc.).
5. Récupérez votre chaîne de connexion PostgreSQL :
   - Allez dans **Project Settings** > **Database**.
   - Sous **Connection string**, sélectionnez **URI** ou **ADO.NET**.
   - Exemple d'URI : `Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.votre_projet;Password=votre_mot_de_passe;SSL Mode=Require;Trust Server Certificate=true;`

---

## 2. Configuration du Backend ASP.NET Core 6

1. Ouvrez le dossier `aspnetcore-backend/` dans Visual Studio 2022 ou VS Code.
2. Ouvrez `appsettings.json` et remplacez la clé `DefaultConnection` par votre chaîne Supabase :
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.votre_projet;Password=VOTRE_MOT_DE_PASSE;SSL Mode=Require;Trust Server Certificate=true;"
   }
   ```
3. Exécutez l'application en ligne de commande :
   ```bash
   cd aspnetcore-backend
   dotnet restore
   dotnet run
   ```
4. L'API démarrera avec Swagger accessible sur : `https://localhost:7145/swagger`

---

## 3. Configuration du Frontend Angular 15

1. Ouvrez le dossier `angular-frontend/`.
2. Installez les dépendances Node.js :
   ```bash
   cd angular-frontend
   npm install
   ```
3. Vérifiez l'URL de l'API dans `src/environments/environment.ts` :
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7145/api'
   };
   ```
4. Lancez le serveur de développement Angular :
   ```bash
   ng serve --open
   ```
5. Rendez-vous sur `http://localhost:4200` pour tester l'application.

---

## 4. Identifiants de Connexion par Défaut

- **Directeur Général (Admin)** : `admin@batipromo.dz` / `promo2026`
- **Conducteur Travaux** : `conducteur@batipromo.dz` / `promo2026`
- **Responsable Caisse** : `caisse@batipromo.dz` / `promo2026`
