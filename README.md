# **Commentium**

**Live Demo**: [Commentium on Azure](http://20.157.206.38:4200)

## **Project Overview**

Commentium is a web-based commenting application designed for users to share and interact with comments and replies in a hierarchical structure. The application includes a robust form validation system, support for HTML formatting in comments, file attachment options, and an interactive comments feed. Data persistence, caching, and event-based logging are also integral features of Commentium.

### **Main Features**

#### **1\. Comment Submission Form**

The comment submission form allows users to submit new comments with various options and validation requirements.

* **Fields and Validation**:  
  * **Username**: Only Latin letters and numbers are allowed.  
  * **Email**: Validates for proper email format.  
  * **Comment Text**: Provides an editor toolbar for inserting HTML tags:  
    * `<a href="" title=""> </a>`  
    * `<code> </code>`  
    * `<i> </i>`  
    * `<strong> </strong>`  
  * HTML tags are restricted to only the approved list; other tags are filtered out. Users can also preview the comment to see how it will appear.  
  * **Captcha**: Generates a new captcha upon every attempt to submit data.  
  * **File Attachment**:  
    * Supports JPG, GIF, PNG, and TXT formats.  
    * Images larger than 320x240 pixels are resized proportionally.  
    * TXT files are accepted only if they are under 100 KB in size.  
    * After attachment, a thumbnail is displayed for review. Users can open the image or text file for a full preview.  
* **Client-Side and Server-Side Validation**:  
  * The form checks for field validity before enabling submission.  
  * If any field is invalid, an error is displayed, and the submission is blocked.

#### **2\. Comments Display and Replies**

The comments display page showcases submitted comments along with their replies.

* **Comment Structure**:  
  * Shows the sender's username, submission date, comment text, and an attached file thumbnail (if available).  
  * Each comment supports nested replies, with no restriction on reply depth.  
* **Replies**:  
  * Each comment includes a button to add a reply, opening the comment submission form in reply mode.  
* **Sorting**:  
  * Users can sort root comments by date, username, and email.  
* **Pagination**:  
  * Comments are paginated, with 25 comments per page for efficient navigation.

#### **3\. Backend Features**

* **Caching**: Server responses are cached to improve performance.  
* **Data Storage**: User and comment data are stored in a relational database.  
* **Event Processing**:  
  * When a comment is created, an event is triggered and pushed to a message queue.  
  * Events are logged for audit and debugging purposes.

## **Getting Started**

Follow these instructions to set up and run Commentium locally using Docker Compose.

### **Prerequisites**

Ensure **Docker** and **Docker Compose** are installed on your machine.

### **Installation**

1. **Clone the Repository**:  
   First, download the project to your computer by running:  
   `git clone https://github.com/ArtemBeloded/Commentium.git`  
2. **Navigate to the Project Folder**:  
   Change into the project directory and open a terminal.  
3. **Build and Start the Application**:  
   In the terminal, run:  
   `docker-compose up --build -d`

This will start all necessary containers for the application.

4. **Check Application Logs**:  
   Once the containers start, check the logs to ensure everything is running correctly. Enter:  
   `docker logs -f commentium-api`  
5. Wait for the log messages `Database is up to date and ready for use.` and `Migration process has finished.` before proceeding.  
6. **Access the Application**:  
   Open a browser and go to `http://localhost:4200` to use the application.  
7. **Stopping the Application**:  
   To stop the application, go back to the terminal in the project directory and run:  
   `docker-compose down`

Thank you for exploring Commentium\!