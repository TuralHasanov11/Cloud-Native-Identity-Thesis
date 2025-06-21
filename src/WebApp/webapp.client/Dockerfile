ARG NODE_VERSION=23.11.0

# Create build stage
FROM node:${NODE_VERSION}-alpine3.20 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files to the working directory
COPY package.json package.json
COPY package-lock.json package-lock.json

# Install dependencies using npm
RUN npm install

# Copy the rest of the application files to the working directory
COPY . .

RUN npm run build

EXPOSE 3000

# Use the lightweight Nginx image from the previous stage for the nginx container
FROM nginx:stable-alpine AS production

# Copy the build application from the previous stage to the Nginx container
COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

# Start Nginx to serve the application
CMD ["nginx", "-g", "daemon off;"]
