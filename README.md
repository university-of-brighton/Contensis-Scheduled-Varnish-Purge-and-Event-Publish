Scheduled Events for Contensis CMS
====

Schedule Varnish Purges and Entry Publishes in a Contensis CMS.

In Contensis 11.3 it isn't possible to schedule an entry to publish at a certain time.
There is also no way to notify Varnish (if you are using a Varnish server) to purge any pages which may use the entry when it is published. This is a way to schedule these events using
* A content type scheduledEvent
* A razorview which queries the scheduledEvent entries based on a time
* An external timed trigger to call the razor view (in this example an Azure Function App with a Timer trigger but could be Heroku or AWS Lambda)

CMS Installation
---

1. Use the Taxomony Manager in the CMS to create a new Taxonomy Node with 4 child nodes:
  * Publish entry
  * Purge page
  * Purge folder
  * Purge folder including subfolders
and make a note of their taxonomy keys.
2. Create a new content type called Scheduled Event giving it a sensible description
3. Save it and append /design to the url of the content editor window which opens up the json editor of the content type.
4. Open scheduledEvent.json and copy from Line 18 (`"entryTitleField": "title",`) to the bottom of the file and copy to the corresponding section of the editor.
5. Save the content type, return to Design view by clicking on the button and publish the content type.
6. Create a new razorview anywhere and copy the contents of run-scheduled-publish-and-purge-events.cshtml into it.
7. Edit your razorview and fill in your details.

You now have a razorview which when visited with the time and correct key as query parameters will perform the action for that time. E.g. if the razorview is installed at /razorviews/run-scheduled-publish-and-purge-events.cshtml then a GET request to https://yoursite/razorviews/run-scheduled-publish-and-purge-events.cshtml?when=2019-08-29T12:30&key=YOURKEY will run any scheduled events for 12.30pm on the 29th August 2019.

Azure Installation
---

1. Create a new Azure Function app and copy the contents of Function1.cs into it. Create 2 new Application settings:
* ApiKey
* RunnerEndpoint
2. fill in with the key you added to the razorview and the url of the razor (without query string).
3. Publish

The Function App will now run every 15 minutes calling the razorview to perform any actions in the scheduled event content type. The cost should be under £1 a month (or free if you are still on free account).

Creating Scheduled Event entries
---

* The title is just so you can identify what this enty does.
* Select the time (NB Contensis currently only allows increments of 15 minutes which is why the calling function only needs to run every 15 minutes).
* Select which type of action
* Add parameters:
  * for page purges give the pages's relative path e.g. `/this/page.aspx`
  * for folder pruges give the folder path without trailing slashes e.g. `/purge/this/folder`
  * for entry publishes give the entry Guid e.g. `88c74739-db11-4ed6-81d8-61fc0f84691f`
