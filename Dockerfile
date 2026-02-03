# Use the official MySQL image which includes auto-initialization logic
FROM mysql:8.0

# Set the root password as you did previously
ENV MYSQL_ROOT_PASSWORD=pass 

# Copy your init script into the specific directory the official image watches
# The official image will automatically run this file on the first volume creation
COPY init.sql /docker-entrypoint-initdb.d/init.sql

# Note: You do not need to manually install mysql-server, sed the bind-address, 
# EXPOSE the port, or set the ENTRYPOINT. The official 'mysql' image 
# handles all of these configurations automatically.