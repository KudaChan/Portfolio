import PropsTypes from 'prop-types'
const ServiceCard = ({ imgURL, label, subtext }) => {
    return (
        <div className='flex-1 sm:w-[350px] w-full rounded-3xl shadow-3xl px-10 py-16 hover:shadow-2xl'>
            <div className='w-11 h-11 flex justify-center items-center bg-coral-red rounded-full'>
                <img
                    src={imgURL}
                    alt={label}
                    width={24}
                    height={24}
                />
            </div>
            <h3 className='mt-5 font-palanquin text-3xl leading-normal font-bold'>{label}</h3>
            <p className='mt-3 font-montserrat break-words text-lg text-slate-gray'>{subtext}</p>
        </div>
    )
}

ServiceCard.propTypes = {
    imgURL: PropsTypes.string.isRequired,
    label: PropsTypes.string.isRequired,
    subtext: PropsTypes.string.isRequired,
}

export default ServiceCard