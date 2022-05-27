import { Stack } from '@mui/material'
import React from 'react'
import { Link } from 'react-router-dom'

export interface AuctionItemProps {
    id: number;
    name: string
    price: number,
    description: string
}

export const AuctionItem = (props: AuctionItemProps) => {
    return (
        <Stack>
            <Link
                key={props.name}
                to={`/auctions/${props.id}`}
            >
                name: {props.name}
                price: {props.price}
                description: {props.description}

            </Link>
        </Stack>
    )
}
