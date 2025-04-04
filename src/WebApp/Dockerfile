ARG NODE_VERSION=23.11.0

# Create build stage
FROM node:${NODE_VERSION}-alpine3.20 AS base

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files to the working directory
COPY src/WebApp/package.json /app/
COPY src/WebApp/package-lock.json /app/

# Install dependencies using npm
RUN npm install && npm cache clean --force

# Copy the rest of the application files to the working directory
COPY src/WebApp/ /app/

# Expose the port the application will run on
EXPOSE 3000
# EXPOSE 3001

# Build the application
# RUN npm run build

# Create a new stage for the production image
# FROM base as build

# Set the working directory inside the container
# WORKDIR /app

# Copy the output from the build stage to the working directory
# COPY --from=build /app/.output ./ 

# Define environment variables
# ENV HOST=0.0.0.0
# ARG NODE_ENV=production
# ENV NODE_ENV=${NODE_ENV}

# Start the application
# CMD ["node", "/app/server/.mjs"]
CMD ["npm", "run", "dev"]