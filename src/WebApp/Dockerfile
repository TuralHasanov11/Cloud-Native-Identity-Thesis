ARG NODE_VERSION=23.11.0

# Create build stage
FROM node:${NODE_VERSION}-alpine3.20 AS base

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files to the working directory
COPY ./src/WebApp/package*.json /app/
# COPY package*.json ./

# Install dependencies using npm
RUN npm install

# Copy the rest of the application files to the working directory
COPY ./src/WebApp/ /app/
# COPY . .

EXPOSE 3000

# Build the application
RUN npm run build-only

# Use the lightweight Nginx image from the previous stage for the nginx container
FROM nginx:stable-alpine as production

# Copy the build application from the previous stage to the Nginx container
COPY --from=base /app/dist/ /usr/share/nginx/html/

# Expose port 3000
EXPOSE 3000

# Start Nginx to serve the application
CMD ["nginx", "-g", "daemon off;"]