# GUIDE DE DÉPLOIEMENT RENDER.COM 🚀
## Stack : ASP.NET Core 6 Web API · Angular 15 · PostgreSQL Supabase

Ce guide vous explique exactement comment déployer gratuitement votre application sur **Render.com** (gratuit, automatisé et sécurisé avec HTTPS).

---

## Méthode 1 : Déploiement Automatique 1-Clic via Blueprint (Recommandée)

Grâce au fichier `render.yaml` présent à la racine de votre projet, Render déploie automatiquement l'ensemble de votre architecture :

1. **Poussez votre code sur GitHub** (ou GitLab).
2. Connectez-vous sur **[dashboard.render.com](https://dashboard.render.com)**.
3. Cliquez sur le bouton **New +** (en haut à droite) et sélectionnez **Blueprint**.
4. Sélectionnez votre dépôt GitHub `batipromo`.
5. Render détecte le fichier `render.yaml` et crée automatiquement :
   - ⚡ **`batipromo-aspnet-api`** (Web Service Docker pour l'API ASP.NET Core 6).
   - 🅰️ **`batipromo-angular-app`** (Static Site pour l'interface Angular 15).
6. Cliquez sur **Apply** : le déploiement se lance !

---

## Méthode 2 : Déploiement Manuel Étape par Étape

Si vous préférez créer les services manuellement sur le tableau de bord Render :

### A. Déployer le Backend ASP.NET Core 6 (Web Service)
1. Sur Render, cliquez sur **New +** > **Web Service**.
2. Connectez votre dépôt GitHub.
3. Donnez un nom : `batipromo-aspnet-api`.
4. **Environment** : Sélectionnez `Docker`.
5. **Dockerfile Path** : `./aspnetcore-backend/Dockerfile`
6. **Docker Build Context** : `./aspnetcore-backend`
7. Dans **Environment Variables**, ajoutez :
   - `ConnectionStrings__DefaultConnection` = `Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.bbldpilwekgdbbnjwslb;Password=Radino@2019@;SSL Mode=Require;Trust Server Certificate=true;`
   - `Jwt__Key` = `BatipromoDzSuperSecretEncryptionKey2026ForAspDotNetCoreJwtAuth!*`
   - `Jwt__Issuer` = `BatipromoDzApi`
   - `Jwt__Audience` = `BatipromoDzAngularClient`
   - `ASPNETCORE_ENVIRONMENT` = `Production`
8. Cliquez sur **Create Web Service**. Vous obtiendrez votre URL API HTTPS (ex: `https://batipromo-aspnet-api.onrender.com`).

---

### B. Déployer le Frontend Angular 15 (Static Site)
1. Sur Render, cliquez sur **New +** > **Static Site**.
2. Connectez le même dépôt GitHub.
3. Donnez un nom : `batipromo-angular-app`.
4. **Build Command** : `cd angular-frontend && npm install && npm run build -- --configuration production`
5. **Publish Directory** : `./angular-frontend/dist`
6. **Règle de Routage SPA Angular (Trè Important !)** :
   - Dans l'onglet **Redirects / Rewrites**, ajoutez une règle :
     - **Type** : `Rewrite`
     - **Source** : `/*`
     - **Destination** : `/index.html`
   *(Cette règle est cruciale pour que le rafraîchissement des pages Angular comme `/dashboard` ou `/chantiers` fonctionne sans erreur 404)*.
7. Cliquez sur **Create Static Site**.

---

## Vérification du Déploiement

- **URL Frontend Angular 15** : `https://batipromo-angular-app.onrender.com`
- **URL Backend API Swagger** : `https://batipromo-aspnet-api.onrender.com/swagger`
- **Healthcheck API** : `https://batipromo-aspnet-api.onrender.com/api/health`
