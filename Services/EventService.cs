using Blazored.LocalStorage;
using PlanIt.Models;

namespace PlanIt.Services
{
    public class EventService
    {
        private readonly ILocalStorageService _localStorage;
        private const string EventsKey = "events";

        public EventService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<(List<Event>, string)> GetEventsAsync()
        {
            try
            {
                var events = await _localStorage.GetItemAsync<List<Event>>(EventsKey);
                return (events ?? new List<Event>(), null);
            }
            catch (Exception ex)
            {
                return (null, $"Failed to load events: {ex.Message}");
            }
        }

        public async Task<string> AddEventAsync(Event newEvent)
        {
            try
            {
                var events = (await GetEventsAsync()).Item1;
                events.Add(newEvent);
                await _localStorage.SetItemAsync(EventsKey, events);
                return null;
            }
            catch (Exception ex)
            {
                return $"Failed to add the event: {ex.Message}";
            }
        }

        public async Task<string> UpdateEventAsync(Event updatedEvent)
        {
            try
            {
                var events = (await GetEventsAsync()).Item1;
                var index = events.FindIndex(e => e.Title == updatedEvent.Title);
                if (index != -1)
                {
                    events[index] = updatedEvent;
                    await _localStorage.SetItemAsync(EventsKey, events);
                    return null;
                }
                return "Event not found.";
            }
            catch (Exception ex)
            {
                return $"Failed to update the event: {ex.Message}";
            }
        }

        public async Task<string> DeleteEventAsync(string title)
        {
            try
            {
                var events = (await GetEventsAsync()).Item1;
                var eventToDelete = events.FirstOrDefault(e => e.Title == title);
                if (eventToDelete != null)
                {
                    events.Remove(eventToDelete);
                    await _localStorage.SetItemAsync(EventsKey, events);
                    return null;
                }
                return "Event not found.";
            }
            catch (Exception ex)
            {
                return $"Failed to delete the event: {ex.Message}";
            }
        }
    }
}
