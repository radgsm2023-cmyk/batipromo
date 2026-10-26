# ==============================================================================
# Dockerfile Multi-Stage pour Frontend Angular 15 avec Serveur Nginx Alpine
# ==============================================================================

# Étape 1 : Build de l'application Angular
FROM node:18-alpine AS build
WORKDIR /app

# Copie des définitions de packages et installation
COPY package*.json ./
RUN npm ci --legacy-peer-deps || npm install

# Copie du code source et compilation de production
COPY . .
RUN npm run build -- --configuration production || npx ng build --configuration production

# Étape 2 : Serveur Web Nginx haute performance
FROM nginx:alpine AS runtime

# Nettoyage du répertoire web par défaut
RUN rm -rf /usr/share/nginx/html/*

# Copie des fichiers compilés de l'étape de build
COPY --from=build /app/dist/* /usr/share/nginx/html/

# Copie de la configuration Nginx avec support SPA
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
