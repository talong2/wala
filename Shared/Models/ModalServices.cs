using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperApp.Services
{
    public interface IModalServices
    {
        Task<ModalBuilder> Build(string _elem, bool _static = true, bool _focus = true);
        Task Clear();
        Task Close(ModalBuilder modal);
        Task Close(string elem);
    }

    public class ModalServices : IModalServices
    {
        private IJSRuntime JS { get; set; }
        public ModalServices(IJSRuntime js)
        {
            JS = js;
        }

        public async Task<ModalBuilder> Build(string _elem, bool _static = true, bool _focus = true)
        {
            ModalBuilder builder = new() { Elem = _elem, Static = _static, Focus = _focus };
            await JS.InvokeVoidAsync("OpenModal", builder.Elem, builder.Static, builder.Focus);

            return builder;
        }

        public async Task Close(ModalBuilder modal)
        {
            var pre = modal.Elem.First();
            if (pre != '#')
                modal.Elem = $"#{modal.Elem}";

            await JS.InvokeVoidAsync("CloseModal", modal.Elem);
        }

        public async Task Close(string elem)
        {
            var pre = elem.First();
            if (pre != '#')
                elem = $"#{elem}";

            await JS.InvokeVoidAsync("CloseModal", elem);
        }

        public async Task Clear()
        {
            await JS.InvokeVoidAsync("ClearModals");
        }
    }

    public class ModalBuilder
    {
        public string Elem { get; set; }
        public bool Static { get; set; }
        public bool Focus { get; set; }
    }

}
