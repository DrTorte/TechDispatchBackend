Tech Dispatch (formerly known as Skiffie!) Backend - for dispatching wireless techs for tower based infrastructure, with time and install zones and IP tracking.<br />

<h2>How to use:</h2>

<ol>
  <li>Upload to ASP.NET compatible server of your choice!</li>
  <li>Update web.config for TechDispatchContext</li>
  <li>Go!</li>
</ol>

<h2>Example API:</h2>

<ul>
<li>/api/login: Log in and retrieve the bearer header. Note that there is no public registration, instead, it has to be done by admin rights.</li>
<li>/Customers: Gets a list of current customers<br />
  Optional parameter: Search, searches through Name/Phone Number/Address and returns a list of customers.</li>
<li>/Appointments: Get a list of current appointments<br />
  Optional parameters example: To/FromDate: Limits the date selection.</li>
</ul>

<h2>Purpose:</h2>

The intent is to create a simple, straightforward tool that allows a user straightforward view of scheduling, and for admins to be able to control everything in one location rather than half a dozen.

More features forthcoming!
