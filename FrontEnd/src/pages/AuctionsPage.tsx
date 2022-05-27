import React from 'react'
import { AuctionItem } from '../components/AuctionItem'
import { PageLayout } from '../components/PageLayout'

export const AuctionsPage = () => {
    const auctions = [
        {
            id: 1,
            name: "auction1",
            price: 100,
            description: "my auction1"
        },
        {
            id: 2,
            name: "auction2",
            price: 200,
            description: "my auction2"
        },
        {
            id: 3,
            name: "auction3",
            price: 300,
            description: "my auction3"
        },
        {
            id: 4,
            name: "auction4",
            price: 400,
            description: "my auction4"
        },
    ]

    return (
        <PageLayout>
            {auctions.map(auction => {
                return (
                    <AuctionItem
                        id={auction.id}
                        name={auction.name}
                        price={auction.price}
                        description={auction.description}
                    />
                )
            })}
        </PageLayout>
    )
}
