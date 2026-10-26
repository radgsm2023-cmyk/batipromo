# GUIDE DOCKER BATIPROMO DZ 🐳

Ce guide vous explique comment lancer l'application conteneurisée sous Docker et Docker Compose.

---

## 1. Architecture Docker Compose (ASP.NET Core 6 + Angular 15 + PostgreSQL)

Le fichier `docker-compose.yml` orchestre 3 services conteneurisés :
1. **`postgres`** : Base de données PostgreSQL 15 alpine préconfigurée avec le schéma `schema_supabase.sql` exécuté automatiquement au démarrage.
2. **`backend`** : API RESTful ASP.NET Core 6 compilée en multi-stage build, exposée sur le port `5000`.
3. **`frontend`** : Application Angular 15 servie par Nginx Alpine haute performance avec support SPA et proxy `/api/`, exposée sur le port `8080`.

---

## 2. Commandes de Démarrage

### Démarrer toute la stack en arrière-plan :
```bash
docker compose up -d --build
```

### Vérifier l'état des conteneurs :
```bash
docker compose ps
```

### Consulter les logs en temps réel :
```bash
# Tous les services
docker compose logs -f

# Uniquement le backend ASP.NET Core
docker compose logs -f backend

# Uniquement la base PostgreSQL
docker compose logs -f postgres
```

### Arrêter les conteneurs :
```bash
docker compose down
```

### Arrêter et supprimer les volumes (réinitialisation complète) :
```bash
docker compose down -v
```

---

## 3. Accès aux Applications après démarrage

- **Frontend Angular 15** : [http://localhost:8080](http://localhost:8080)
- **API Swagger ASP.NET Core 6** : [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **Healthcheck API** : [http://localhost:5000/api/health](http://localhost:5000/api/health)
- **Base PostgreSQL** : `localhost:5432` (Utilisateur : `postgres`, Mot de passe : `postgres_password_2026`, Base : `batipromo`)

---

## 4. Lancer l'Application Node.js / React (Optionnel)

Si vous souhaitez lancer la version Node.js / React existante sous Docker :
```bash
docker build -t batipromo-node .
docker run -p 3000:3000 batipromo-node
```
Accès sur : [http://localhost:3000](http://localhost:3000)
