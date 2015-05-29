using System;
using Android.Widget;
using OfficeSolutions.SharePoint.Model;
using Android.App;
using Android.Views;

namespace OfficeSolutions.SharePoint
{
	public class ListItemAdapter : BaseAdapter<ListItem>
	{
		ListItem[] items;
		Activity context;

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].Title;
			return view;
		}

		public override int Count {
			get {
				return items.Length;
			}
		}


		public override ListItem this [int index] {
			get {
				return items[index];
			}
		}

		public ListItemAdapter (Activity context, ListItem[] items) : base() {
			this.context = context;
			this.items = items;
		}
	}
}

