import PropsTypes from 'prop-types'
import { star } from '../assets/icons'


const ReviewCard = ({ imgURL, customerName, rating, feedback }) => {
    return (
        <div className='flex flex-col justify-center items-center'>
            <img
                src={imgURL}
                alt='customer'
                className='rounded-full object-cover w-[120px] h-[120px]'
            />
            <p className='mt-6 max-w-sm text-center info-text'>{feedback}</p>
            <div className='mt-3 flex justify-center items-center gap-2.5'>
                <img
                    src={star}
                    width={24}
                    height={24}
                    className='object-contain m-0'
                />
                <p className='text-xl font-montserrat text-slate-gray'>({rating})</p>
            </div>
            <h3 className='text-3xl font-palanquin text-coral-red mt-1 text-center font-bold'>{customerName }</h3>
        </div>
    )
}

ReviewCard.propTypes = {
    imgURL: PropsTypes.string.isRequired,
    customerName: PropsTypes.string.isRequired,
    rating: PropsTypes.number.isRequired,
    feedback: PropsTypes.string.isRequired,
}

export default ReviewCard