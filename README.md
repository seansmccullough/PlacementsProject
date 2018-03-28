# PlacementsProject

Please keep in mind I'm a backend software engineering.  I'm not a web developer or designer, and I have limited experience with web development and Javascript.

## How to run:
1. Download and install Visual Studio Community and the "ASP.NET and web development" Workload https://www.visualstudio.com/vs/community/
2. Clone this repository
3. Open PlacementsProject.sln in Visual Studio, and hit run.  The database will be created and seeded automatically.

## Placements.io Coding Challenge
Given a set of line-items, how would you go about writing a web-app that allows users to
maintain an adjustable invoice?
Please see attached for example line-item data.
Below is a list of some expected behaviors and use cases. Pick some from bucket 1 and at
least one from bucket 2:

The features I implements are in bold.

### Bucket 1: ###
- **The user should be able browse through the line-item data as either a list or table (ie.
pagination or infinite-scrolling).**
  - Both the LineItem Index page and Campaign Index page are paginated
- **The user should be able to edit line-item "adjustments".**
  - Users can update a LineItem's adjustment from the LineItem detail page
- **The user should be able to see each line-item's billable amount (sub-total = actuals +
adjustments).**
  - Users can view a LineItems billable amount on both the LineItem Index page and LineItem detail page
- **The user should be able to see sub-totals grouped by campaign (line-items grouped by their
parent campaign).**
  - Users can filter LineItems by Campaign using the search box on the LineItem index page.  The totals at the bottom are updated accordingly
- **The user should be able to see the invoice grand-total (sum of each line-item's billable
amount).**
- Multiple users should be able to edit the same invoice concurrently.
- **The user should be able to sort the data.**
  - User can sort LineItems by all properties on the LineItem Index page, and Campaigned by all Campaign properties on the Campaign Index page
- The user should be able to browse/filter/sort the invoice history, as well.
- The user should be able to output the invoice to *.CSV, *.XLS, etc.
- The user should be able to customize the layout.
- **The user should be able flag individual line-items as "reviewed" (meaning they are disabled
from further editing).**
  - Users can mark LineItems as reviewed from the LineItem Detail page.  Once a LineItem has been marked as reviewed, it and it's Comments and Adjustments cannot be modified
- **The user should be able flag "campaigns" as being reviewed, as well.**
  - Users can mark Campaigns as Reviewed from the Campaign Detail page.  Once a Campaign is marked as reviewed, the Campaign, its LineItems, and Comments and Adjustments associated with its LineItems cannot be modified.
- The user should be able to archive line-items

### Bucket 2: ###
- An integration into an external service that makes sense (eg. a currency conversion service,
an export to Amazon S3, etc)
- **The user should be able to filter the data (ie. by campaign name, etc., should affect the
grand-total).**
  - LineItems can be filtered by Campaign name via the search box on the LineItem Index page.  The totals at the bottom of the page are updated to include the current filter
- The user should be able to share and reuse filters between users.
- **The user should be able to add comments on an individual line-item.**
  - Users can create, edit, and delete (their own) comments  on LineItems that haven't been marked as Reviewed
- **The user should be able to see a history of all the adjustments/comments/changes/etc. made
to the invoice by different users.**
  - Users can see the history of all Adjustments and Comments fron a LineItem on the LineItem Detail page.
  
  ### Other features I implemented: ###
  - Users can delete LineItems
  - Users can edit Campaign name

## Areas for Improvement ##
- Unit tests!  I spent a significant amount of time on this project, and did not get around to unit testing.  I unit test almost all my code at work
- LineItemController.Index() is open to possible SQL injection via the searchString parameter
- Add client-side validation for creating Adjustments.  Right now, the page will let you submit the form with AdjustmentAmount > BookedAmount.  The backend validation catches this, but client side validation would have been ideal
- Implement LineItem create and Campaign create/delete.
- Show LineItems associated with Campaign on Campaign Detail page
