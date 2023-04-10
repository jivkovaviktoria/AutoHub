import {useState} from "react";
import {PriceFilter} from "../../filters/priceFilter/PriceFilter";

export const Filter = ({onFilter}) => {
    const [isOpen, setIsOpen] = useState(false);

    const closeHandler = () => {
        setIsOpen(false);
    }

    return (
        <>
            <button onClick={() => setIsOpen(true)}>Price</button>
            {isOpen && <PriceFilter onFilter={onFilter} onClose={closeHandler}/>}
        </>
    );
}
