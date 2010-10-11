using System;
using MassTransit.NinjectIntegration;
using MassTransit.Services.Routing.Configuration;
using MassTransit.Transports.Msmq;

namespace Apics.Messaging
{
    public abstract class MessagingModuleBase : MassTransitModuleBase
    {
        private readonly Uri endpoint;

        protected MessagingModuleBase( Uri endpoint )
            : base( typeof( MsmqEndpoint ) )
        {
            if( endpoint == null )
                throw new ArgumentNullException( "endpoint" );

            this.endpoint = endpoint;
            MsmqEndpointConfigurator.Defaults( config => { config.CreateMissingQueues = true; } );
        }

        public override void Load( )
        {
            base.Load( );

            RegisterServiceBus( this.endpoint, x =>
                x.ConfigureService<RoutingConfigurator>( rs => ConfigureRoutes( rs, this.endpoint ) ) );
        }

        protected abstract void ConfigureRoutes( RoutingConfigurator rs, Uri endpoint );
    }
}
