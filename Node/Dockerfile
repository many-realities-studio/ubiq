FROM node:18-alpine
# Create app directory
WORKDIR /usr/src/app
# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY ubiq ./
COPY package*.json ./
RUN npm install
# If you are building your code for production
# RUN npm ci --omit=dev
# Bundle app source
COPY . .
COPY config samples ./
RUN npm install
EXPOSE 8010
EXPOSE 8009
# ENTRYPOINT [ "/bin/sh" ]
CMD [ "node", "app.js" ]