# ==============================================================================
# BATIPROMO DZ - Dockerfile Production Multi-Stage (Node.js & React)
# ==============================================================================

FROM node:22-alpine AS builder
WORKDIR /app

# Copie des manifestes et installation
COPY package*.json ./
RUN npm ci || npm install

# Copie du code source et build de l'interface client Vite
COPY . .
RUN npm run build

# Étape d'exécution minimale
FROM node:22-alpine AS runner
WORKDIR /app

ENV NODE_ENV=production
ENV PORT=3000
ENV USE_SQLITE=true

COPY package*.json ./
RUN npm ci --only=production || npm install --production

# Copie des artefacts compilés et du serveur
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/server ./server
COPY --from=builder /app/server.ts ./server.ts
COPY --from=builder /app/batipromo.db ./batipromo.db

EXPOSE 3000

CMD ["npx", "tsx", "server.ts"]
