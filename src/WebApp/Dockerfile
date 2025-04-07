ARG NODE_VERSION=23.11.0

# Create build stage
FROM node:${NODE_VERSION}-alpine3.20 AS base

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files to the working directory
COPY ./package.json /app/
COPY ./package-lock.json /app/

# Install dependencies using npm
RUN npm install && npm cache clean --force

# Copy the rest of the application files to the working directory
COPY ./ /app/

# Build the application
RUN npm run build

# Use the lightweight Nginx image from the previous stage for the nginx container
FROM nginx:stable-alpine as production

# Copy the build application from the previous stage to the Nginx container
COPY --from=base /app/dist/ /usr/share/nginx/html/

# Copy the nginx configuration file
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Start Nginx to serve the application
CMD ["nginx", "-g", "daemon off;"]